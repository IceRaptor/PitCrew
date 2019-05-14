# PitCrew
This mod for the [HBS BattleTech](http://battletechgame.com/) game breathes life into the technical crews responsible for keeping your company's BattleMechs in fighting shape. The crew is harder to manage, requires a salary, and can occasionally make mistakes that will set back repairs or damage stored items. They are also mercenaries, and can leave at end of a contract or when someone else makes them a better offer! You'll always have Wang and a handful of his chosen favorites, but that may not help if you have a full mechbay of repairs and customizations!

## Feature Overview

 * You can hire a crew of mercenary MechTechs to increase your overall tech points. They will leave after their contract ends.
 * MechTechs can be head-hunted by other mercenary companies on inhabited planets. There is small chance each day they will leave, and you'll have to travel to another planet to re-hire them.
 * Repairs can be botched, damaging components on the BattleMech being repaired. Components that are already damaged can be destroyed.
 * Mechs ready for deployment require a daily cost in mech points. If this isn't paid, there's a chance for components or armor to be damaged.
 * Stored parts require monthly maintenance, and can be damaged in that maintenance.

## MechTech Crews
By default, your company has Wang and 2-3 MechTechs that are founding members. They take a cut of the profits, and if someone runs off for better pastures, Wang finds a replacement. However, they will be stretched extremely thin if you suddenly find yourself with multiple mechs in need of repair.

Anytime you're at a planet, you can speak with Wang and ask him to hire additional techs. You can choose the size of the crew you want to hire, as well as their general expertise level.

The larger the crew, the easier it is to maintain multiple mechbays and simultaneous fixes. The following sizes of crew can be hired:

| Crew Size | Members | Tech Point Bonus | Notes |
| -- | -- | -- | -- |
| Tiny | 1-3 | +2 | |
| Small | 3-5 | +3 | Requires Beta Hub |
| Medium | 5-8 | +5 | Requires Beta Hub |
| Large | 8-12 | +7 | Requires Gamma Hub |
| Huge | 12-16 | +10 | Requires Gamma Hub |

The overall skill of these techs is given by their *experience* rating. The rating is an overall sum which can vary due to many factors. A rookie crew could be techs that have only tinkered with equipment before, or it could be a crew of veterans that just don't get along with each other.

| Crew Rating | Repair Fail% | Maint. Fail% | Tech Point Multiplier | Monthly Cost |
| -- | -- | -- | -- | -- |
| Rookie | 99% | 99% | x0.4 | 100 c-bills |
| Regular | 99% | 99% | x0.7 | 1,000 c-bills |
| Veteran | 99% | 99% | x1 | 5,000 c-bills |
| Elite | 99% | 99% | x1.3 | 25,000 c-bills |
| Legendary | 99% | 99% | x1.7 | 375,000 c-bills |

### Morale and Lifestyle

MechTechs are people too, and benefit (or suffer!) from their working environment. A company with high morale will rub off on the hired crews, making them more productive and less likely to leaving in the middle of a contract.


## DEVELOPER NOTES

our crew size determines your total # of techpoints.
* Each mechbay with an active mech requires 1 point per day for maintenance
* Any surplus points can go towards repairs/changes.
* Techpoints are multiplied by crew skill modifier (rookie, vet, etc), morale modifier (spartan -50%, extrav +50%)
* Upgrades provide a flat bonus to every repair/change currently happening
So if you have 20 tech points from crew, who are elites and extravagant, you might get 40 tech points
If you have all 18 bays open with mechs in them that's 40 - 18 = 22 points for repairs/changes
Automation/harness/machine shop would add their bonus directly to each of those,  so a +3 yields 10,10,11 points.
If you drop to 1 active job though, you're only getting 22 + 3 points
ChassisTags will also factor in; omni is either a reduction in cost and/or tech point bonus on that particular model
