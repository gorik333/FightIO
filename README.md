# FightIO
3D fight game with simple AI.

A prototype in which there are enemies (bots) and a player.

There are two types of levels:
The first - it has n number of enemies, and n number of allies, including the player. They collect food for a certain time, then move to the center and start fighting.
Their health depends on how much food they consume.
There are two types of food. Simple, and small models of themselves. When picking up food, their size increases.

The second - it has one enemy, and the player. At the specified interval, spawns are neutral men who can be added to their army by the first character who touches. And depending
whoever it was, the neutral man becomes either an enemy or an ally. They also change color to match the main character, and follow him. At the end they move to the center
and start to fight. Everyone has the same amount of health.

  1. The winners are those who killed all the opposite team.
  2. When killing, character spawns particles, depending on their color.
  3. You can customize the amount of food appearing, enemies, spawn interval, enemy IQ coefficient, duration of periods.
  4. The character is a Ragdoll.
  5. Physics (addforce, rigidbody, physic materials, colliders, triggers)
  6. There is a system of movement of enemies to a random point on the map, and movement to food.

Gameplay video:
https://youtu.be/7aA9XYBkmws
