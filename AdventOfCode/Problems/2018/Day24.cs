using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.Problems._2018
{
    internal class Army
    {
        internal int GroupNumber;
        internal ArmyTeam Team;
        internal int UnitCount;
        internal int UnitHP;
        internal int Damage;
        internal DamageType Type;
        internal int Initiative;
        internal DamageType[] Immunity;
        internal DamageType[] Weaknesses;
        internal int EffectivePower
        {
            get
            {
                return UnitCount * Damage;
            }
        }
        public override string ToString()
        {
            return Team.ToString() + " Group " + GroupNumber;
        }
        internal int PotentialDamageFrom(Army otherArmy)
        {
            if (Weaknesses.Contains(otherArmy.Type))
            {
                return otherArmy.Damage * 2;
            } else if (Immunity.Contains(otherArmy.Type))
            {
                return 0;
            } else
            {
                return otherArmy.Damage;
            }
        }

        internal Army(ArmyTeam team, int groupNum, string s)
        {
            GroupNumber = groupNum;
            Team = team;
            Regex armyParse = new Regex(@"(\d+) units each with (\d+) hit points (\(.*?\))?\s?with an attack that does (\d+) (\w+) damage at initiative (\d+)");
            var match = armyParse.Match(s);

            UnitCount = int.Parse(match.Groups[1].Value);
            UnitHP = int.Parse(match.Groups[2].Value);

            var weaknessImmuneList = match.Groups[3].Value;

            weaknessImmuneList = Regex.Replace(weaknessImmuneList, @"[,;\(\)]", "");

            var parts = weaknessImmuneList.Split(' ');
            var curType = "";
            var validTypes = "radiation|fire|cold|slashing|bludgeoning".Split('|');
            List<string> weakList = new List<string>();
            List<string> immuneList = new List<string>();
            foreach (var p in parts)
            {
                if (p.Equals("immune"))
                {
                    curType = p;
                } else if (p.Equals("weak"))
                {
                    curType = p;
                }

                if (validTypes.Contains(p))
                {
                    switch(curType)
                    {
                        case "immune":
                            immuneList.Add(p);
                            break;
                        case "weak":
                            weakList.Add(p);
                            break;
                    }
                }
            }

            Weaknesses = weakList.Select(w => (DamageType)Enum.Parse(typeof(DamageType), w.ToUpper())).ToArray();
            Immunity = immuneList.Select(w => (DamageType)Enum.Parse(typeof(DamageType), w.ToUpper())).ToArray();

            Damage = int.Parse(match.Groups[4].Value);
            Type = (DamageType)Enum.Parse(typeof(DamageType), match.Groups[5].Value.ToUpper());

            Initiative = int.Parse(match.Groups[6].Value);
        }

        internal void TakeDamageFrom(Army attacker)
        {
            var amount = attacker.Damage * attacker.UnitCount;

            if (Immunity.Contains(attacker.Type))
            {
                /* no damage from immune types */
                amount = 0;
            }

            if (Weaknesses.Contains(attacker.Type))
            {
                amount *= 2;
            }

            while (amount > UnitHP && UnitCount > 0)
            {
                UnitCount--;
                amount -= UnitHP;
            }
        }
    }

    enum DamageType
    {
        RADIATION, FIRE, COLD, SLASHING, BLUDGEONING
    }

    enum ArmyTeam
    {
        IMMUNE, INFECTION
    }



    internal class Day24 : BaseProblem
    {
        List<Army> ImmuneArmies = new List<Army>();
        List<Army> InfectionArmies = new List<Army>();
        List<Army> AllArmies = new List<Army>();

        public Day24() : base(2018, 24)
        {
            ArmyTeam currentTeam = ArmyTeam.IMMUNE;
            foreach(var s in InputLines)
            {
                if (s.Equals(string.Empty))
                {
                    continue;
                }
                if (s.StartsWith("Immune"))
                {
                    currentTeam = ArmyTeam.IMMUNE;
                } else if (s.StartsWith("Infection"))
                {
                    currentTeam = ArmyTeam.INFECTION;
                } else
                {
                    if (currentTeam == ArmyTeam.IMMUNE)
                    {
                        Army a = new Army(currentTeam, ImmuneArmies.Count + 1, s);
                        ImmuneArmies.Add(a);
                        AllArmies.Add(a);
                    } else
                    {
                        Army a = new Army(currentTeam, InfectionArmies.Count + 1, s);
                        InfectionArmies.Add(a);
                        AllArmies.Add(a);
                    }
                }
            }
        }

        void Fight()
        {
            /* Phase 1, target selection */
            Dictionary<Army, Army> Targets = new Dictionary<Army, Army>();
            foreach(var army in AllArmies.OrderByDescending(a => a.EffectivePower).ThenByDescending(a => a.Initiative))
            {
                Army newTarget;
                if (army.Team == ArmyTeam.IMMUNE)
                {
                    newTarget = InfectionArmies.Where(a => Targets.ContainsValue(a) == false)
                                                .OrderByDescending(a => a.PotentialDamageFrom(army))
                                                .ThenByDescending(a => a.EffectivePower)
                                                .ThenByDescending(a => a.Initiative)
                                                .FirstOrDefault();
                } else
                {
                    newTarget = ImmuneArmies.Where(a => Targets.ContainsValue(a) == false)
                                                .OrderByDescending(a => a.PotentialDamageFrom(army))
                                                .ThenByDescending(a => a.EffectivePower)
                                                .ThenByDescending(a => a.Initiative)
                                                .FirstOrDefault();
                }
                if (newTarget != null)
                {
                    Targets.Add(army, newTarget);
                }
            }

            foreach (var curArmy in Targets.Keys.OrderByDescending(k => k.Initiative))
            {
                Targets[curArmy].TakeDamageFrom(curArmy);
            }

            /* prune the dead */
            ImmuneArmies.RemoveAll(a => a.UnitCount <= 0);
            InfectionArmies.RemoveAll(a => a.UnitCount <= 0);
            AllArmies.RemoveAll(a => a.UnitCount <= 0);
        }

        /* (\d+) units each with (\d+) hit points (\(.*?\))? with an attack that does (\d+) (\w+) damage at initiative (\d+) */
        /* radiation|fire|cold|slashing|bludgeoning */
        internal override string SolvePart1()
        {
            while (ImmuneArmies.Count > 0 && InfectionArmies.Count > 0)
            {
                Fight();
            }

            return (ImmuneArmies.Sum(a => a.UnitCount) + InfectionArmies.Sum(a => a.UnitCount)).ToString();
        }

        internal override string SolvePart2()
        {
            return "";
        }
    }
}
