### Zuma Game in C#

**Description**:  
This project is a C# implementation of a Zuma-style game, where players aim to shoot balls to match three or more of the same color, preventing the line of balls from reaching the end of the path.

**Features**:

1. **Bezier Curves**:  
    - The game's paths are generated using Bezier curves, providing smooth and visually appealing trajectories for the balls.  
    - There are three main Bezier curves (`obj`, `obj2`, `obj3`), each representing a segment of the track.  
    - Control points for these curves can be modified to change the paths dynamically.

2. **Ball Mechanics**:  
    - Balls are represented by the `ball` class, which includes properties such as position, type (color), and curve progression.  
    - Balls move along the Bezier curves based on a parameter `tp`, which increments over time, causing the balls to progress along the path.  
    - Collision detection is implemented to manage ball interactions and matches. When a ball of the same type hits another, it can trigger a chain reaction of matching and removal.

3. **User Interaction**:  
    - Players control a frog located at the center of the screen, which shoots balls towards the paths.  
    - The direction and speed of the shot balls are calculated based on user clicks.  
    - Control points of the Bezier curves can be modified in the "points" mode using mouse drag operations.

4. **Graphics and Animations**:  
    - The game uses double buffering to minimize flickering and provide smooth animations.  
    - Background, frog, and ball images are loaded from external files (`bg.jpg`, `frogt.png`, `bball.png`, `rball.png`, `gball.jpg`).  
    - The game scene is rendered in the `Drawscene` method, which includes drawing the background, frog, and balls at their respective positions.

5. **Modes**:  
    - The game features different modes controlled by keyboard inputs:  
        - **Speed Mode**: Adjusts the speed of ball movement.  
        - **Points Mode**: Allows modification of Bezier curve control points.  
        - **Freeze Mode**: Controls the frequency of new ball generation and progression.

**Technical Details**:

- **Bezier Curve Calculation**: Uses factorial and binomial coefficient functions to calculate the Bezier blending functions, determining the position of balls on the curve at any given time.  
- **Collision Detection**: Determines when a shot ball intersects with a ball on the path, initiating the match-checking logic.  
- **Ball Movement**: Implements Bresenham's line algorithm to calculate the trajectory and smooth movement of the shot balls.

**Getting Started**:  
To run the game, ensure that you have the following files in the project directory:  
- `bg.jpg` (Background image)  
- `frogt.png` (Frog image)  
- `bball.png` (Blue ball image)  
- `rball.png` (Red ball image)  
- `gball.jpg` (Green ball image)  

Compile and run the project using a C# IDE such as Visual Studio.

**Controls**:

- **Mouse**: Click to shoot balls.  
- **Keyboard**:  
    - `Up/Down`: Change mode.  
    - `Left/Right` (in Speed Mode): Adjust speed.  
    - `Left/Right` (in Points Mode): Switch between control point and drag modes.  
    - `Left/Right` (in Freeze Mode): Adjust freeze level.

![Zuma Game Screenshot](/Screenshot (30).png)

Enjoy playing and customizing the Zuma game! Feel free to contribute by adding new features, optimizing the code, or improving the graphics.
