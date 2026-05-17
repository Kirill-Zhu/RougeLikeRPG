#**RogueLikeRPG**
---------------
<img width="1024" height="1024" alt="Icon" src="https://github.com/user-attachments/assets/c996910e-05e4-4a96-bf7a-2e990c5afd43" />
##Описание
----------------------
Стек : Zenject, R3, Addressables, DoTween, UnitTask, IronSource/LevelPlay, UnityCloud, FMOD

Это классический Rogue like RPG основанный на перепрохождении и прокачке персонажа после смертри. В игре реализованы несколько механик прокачки

1: Выбор персонажа который имеет свои особоенности(Рыцарь, маг и.т.д)

<img width="632" height="578" alt="Mage" src="https://github.com/user-attachments/assets/0d167754-56e8-487b-92e5-acb1b3adf382" />
<img width="699" height="596" alt="Foxy" src="https://github.com/user-attachments/assets/31c4fca5-1942-4997-81bf-1014827406ca" />

2: Прокачка непосредтвенно особенностей персонажа с каждым lvlUp(У рыцаря "Удар мечом" "Щит" и " fireBall"   у мага -  "Энергетический щит " "Ледяная буря" "Ледяные осколки"
<img width="1860" height="1001" alt="LvlUp" src="https://github.com/user-attachments/assets/d175545e-3d64-4cc4-94cc-2e97b491472b" />

3: Подбор на карте автокастеров которые не зависят от класса персонажа (Взрыв, отталкивание, выстрелы по горизонали и вертикали)
На карте выглядят как книги
<img width="207" height="229" alt="AutacsterPickUp" src="https://github.com/user-attachments/assets/c008f02a-92e8-4e58-b1c4-f40ec753db64" />

Каждый автокастер иммет свой тип урона, эффект, VFX и SFX 
<img width="427" height="196" alt="Autocaster VFX" src="https://github.com/user-attachments/assets/b101b00f-7b2c-4de6-8f26-3d6d966e3110" />
Можно собрать несколько автокастеров 
Внизу есть UI показывающий кулдауны каждого из автокастеров
<img width="323" height="91" alt="Auocaster UI" src="https://github.com/user-attachments/assets/12bbd958-3000-4b9f-9f6e-60d371358d1f" />

##**Механики урона**

В игре реализованы 3 типа урона 
**Физический** - белый
**Огненный** - оранжевый
**Ледяной** - синий

<img width="601" height="348" alt="Damage" src="https://github.com/user-attachments/assets/d60ffdb0-5e96-4923-8d58-ff8a06be9480" />

Каждое оружие может быть прокачано определенным типо урона либо ящиков c экипировкой на карте либо в магазине.
**Ящик**
<img width="307" height="187" alt="Chest" src="https://github.com/user-attachments/assets/fdd9e7e4-436f-4485-b576-d667c9109108" />
**Итем из ящика**

<img width="534" height="630" alt="ChestCard" src="https://github.com/user-attachments/assets/baf1ba51-b722-4ed0-98f9-55a2763e5b12" />

Каждый персонаж и кадая сущность имеет свои типы урона и свою защиту или уязывимость к кажому типу урон.
**Мгазин**

После каждой сессии игрок получает валюту в виде монет, которую он может потратить в магазине улучшив (Скорость бега, здоровье, ману, тип урона, сопротивления к урону).

<img width="1063" height="578" alt="Shop" src="https://github.com/user-attachments/assets/11ece1a7-89f4-48c4-81f5-ce5a7fb9d2fa" />


