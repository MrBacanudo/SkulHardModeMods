# Custom Items mod for Skul: the Hero Slayer

(Code by [MrBacanudo](https://www.youtube.com/@MrBacanudo/videos), art by [Haxa](https://www.youtube.com/@HaxaPlays/videos))

This mod contains both the foundation for new custom items in the game, and a pack of items to increase diversity!

The initial release has:

* 4 Commons
* 4 Rares
* 3 Uniques
* 3 Omens
* 4 Legendaries

## About the items

The description of the items is left outside this README on purpose, in order to keep things fresh for the player.

However, here are some teasers:

1. Two effects that were never seen in the world of Skul - one is a mysterious Legendary!
2. A _very_ powerful Omen that calls back to two items that were removed from the game
3. Multiple enablers of **multi-status** runs
4. Two enablers of **mixed-damage builds** at low rarities
5. A common masterpiece item with a powerful transformation
6. A legendary item called **Quindent of Sadism** designed by Beelz
7. An enabler and payoff for defensive builds
8. Two powerful legendaries for all-in strategies
9. ... and still many more items!

## Making your own custom items

In the future, there will be a more streamlined process for adding custom items, especially for non-programmers.
For programmers, there are two ways:

1. Fork [the mod repository](https://github.com/MrBacanudo/SkulHardModeMods) and extend the mod with your custom items
    - Easy to get started – recommended way for prototyping
    - **Not Recommended** 
2. Add this mod as dependency of yours 
    - In BepInEx for the order of initialization, on the Assembly for the build, and on the Thunderstore package for distribution
    - **No patching necessary**: You can just append your custom items to `CustomItems.Items`, and they will be added to the game!
    - Template project coming soon - Contributions welcome!
