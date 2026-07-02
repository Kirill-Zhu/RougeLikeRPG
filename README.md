# **RogueLikeRPG**
---------------
<img width="1024" height="1024" alt="Icon" src="https://github.com/user-attachments/assets/c996910e-05e4-4a96-bf7a-2e990c5afd43" />

## Описание
>Made by Kirill Zhuravlev
-----------------
### Стек : Zenject, R3, Addressables, DoTween, UnitTask, IronSource/LevelPlay, UnityCloud, FMOD

### Платфомы : Windows :house:, Linux :penguin:, Android :robot:
### Управление  Keyboard :keyboard:, GamePad :joystick:, Touch :raised_hand_with_fingers_splayed:
------------------



This is a classic roguelike RPG based on replayability and character progression after death. The game features several progression mechanics.

>### 1: Character selection featuring unique traits for each class (Knight, Mage, etc.)

<img width="632" height="578" alt="Mage" src="https://github.com/user-attachments/assets/0d167754-56e8-487b-92e5-acb1b3adf382" />
<img width="699" height="596" alt="Foxy" src="https://github.com/user-attachments/assets/31c4fca5-1942-4997-81bf-1014827406ca" />

-----------

>### 2: Upgrading character-specific traits with every level-up.

> - The Knight features "Sword Slash", "Shield", and "Fireball".
> - The Mage features "Energy Shield", "Blizzard", and "Ice Shards".

<img width="1860" height="1001" alt="LvlUp" src="https://github.com/user-attachments/assets/d175545e-3d64-4cc4-94cc-2e97b491472b" />
  
  Example of a mid-round Level Up.

-----------

>3: Picking up auto-casters on the map that are independent of the character class (Explosion, Repulsion, Horizontal and Vertical Shots).

>You can stack multiple auto-casters. 

<img width="207" height="229" alt="AutacsterPickUp" src="https://github.com/user-attachments/assets/c008f02a-92e8-4e58-b1c4-f40ec753db64" />

They look like books on the map.

-----------



>Each auto-caster has its own damage type, effect, VFX, and SFX.

<img width="427" height="196" alt="Autocaster VFX" src="https://github.com/user-attachments/assets/b101b00f-7b2c-4de6-8f26-3d6d966e3110" />

Example of  **"Green Blow"**  *auto-caster*
- Deals **Physical damage** и **Knock back** enemies


<img width="323" height="91" alt="Auocaster UI" src="https://github.com/user-attachments/assets/12bbd958-3000-4b9f-9f6e-60d371358d1f" />

At the bottom, there is a UI showing the cooldown of each active *auto-caster*



## **Damage mechanics.**

>The game features 3 damage types.
### - **Physical Damage** - white
### - **Fire Damage** - orange
### - **Ice Damage** - blue
>Every character and entity has their own damage types, along with specific defense or vulnerability to each *damage type*.

<img width="601" height="348" alt="Damage" src="https://github.com/user-attachments/assets/d60ffdb0-5e96-4923-8d58-ff8a06be9480" />

The damage type dealt animates above the enemy's head and is highlighted with a corresponding *color*.

------

>### Each weapon can be upgraded with a specific damage type either from *equipment crates* found on the map or at the *shop*.

<img width="307" height="187" alt="Chest" src="https://github.com/user-attachments/assets/fdd9e7e4-436f-4485-b576-d667c9109108" />

Equipment crate.

<img width="534" height="630" alt="ChestCard" src="https://github.com/user-attachments/assets/baf1ba51-b722-4ed0-98f9-55a2763e5b12" />

Item from equipment crate.


**Shop**

After each session, the player receives currency in the form of coins, which they can spend in the shop to upgrade their attributes (Movement Speed, Health, Mana, Damage Type, Damage Resistances).

<img width="1063" height="578" alt="Shop" src="https://github.com/user-attachments/assets/11ece1a7-89f4-48c4-81f5-ce5a7fb9d2fa" />


