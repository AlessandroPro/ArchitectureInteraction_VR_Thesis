# VR Thesis Project

This program was developed as part of an undergraduate research project in my final year studying Computer Science at Ryerson University. It explores how a small-scale model of a building can be interacted with to help navigate a full-scale model of the same building.
Pasted below are the figures and abstract from the paper I wrote, which describe the research, VR application, and user study. 

This was developed in Unity using the HTC Vive and SteamVR. <br>
All relevant code is within the scripts in /Assets/Scripts/. <br>
All 3D assets (except for the model table), and the shaders used for the cutting plane, were downloaded from third-party sources.


### 'Designing an Architectural Model Explorer for Intuitive Interaction and Navigation in Virtual Reality'

#### Abstract 

The availability of modern virtual reality (VR) technology provides increasingly immersive ways for the average person to observe and view building models. In this paper, prior research of multi-scale virtual environments, interaction and navigation methods, and existing commercial software were combined and extended to design a simple and intuitive architectural model explorer in VR. Specifically, this paper aimed at understanding how such a program should interface with a naïve user, that is, someone inexperienced with VR and architecture. A user study was conducted with 10 participants to observe the holistic effectiveness of the design decisions of the created program, such as the navigational benefits of the moveable small-scale model and its WIM-like interface.

The program was developed in Unity for the HTC Vive VR headset and was designed to include a full-scale model of a building and a small-scale 1:10 version of the same building within the same virtual space. The small-scale model can be moved wherever the user chooses or hidden completely. It can also be rotated and sectioned-off using a cutting plane. The user can teleport within the full-scale model by selecting a point on the ground around them or by choosing a point in the small-scale model. All interaction methods, including navigation, are initiated using a remote point and grab technique, combined with visual cues like colour changes, highlights, and affordances. Once something is grabbed, it can be manipulated using either hand motion or directional buttons on the controller’s trackpad. With multiple options for teleportation and manipulation techniques, each user was given a navigational task and recorded and surveyed to understand their preferences and/or frustrations.

The results of the user study showed that the design of the program was mostly a success and that the small-scale building model used is a great multi-purpose tool for navigational aid. Being able to teleport both locally and using the small-scale model is advantageous. Pointing and grabbing for selection is useful for self-discovery and quick learning, and indication through magic/affordance is especially important. Inexperienced VR users instinctively used motion for movement and manipulation. It was therefore good enough for general usage, especially since users did not care to use the directional buttons for more precision.



<img width="944" alt="Screen Shot 2020-01-12 at 10 28 14 PM" src="https://user-images.githubusercontent.com/15040875/72232025-a9484200-358c-11ea-8739-1560680a9431.png">

<img width="952" alt="Screen Shot 2020-01-12 at 10 28 39 PM" src="https://user-images.githubusercontent.com/15040875/72231785-54f09280-358b-11ea-8d9a-d5af5cbcf7c2.png">

<img width="823" alt="Screen Shot 2020-01-12 at 10 44 14 PM" src="https://user-images.githubusercontent.com/15040875/72232235-b9145600-358d-11ea-9735-5f9571d181fe.png">

<img width="932" alt="Screen Shot 2020-01-12 at 10 43 45 PM" src="https://user-images.githubusercontent.com/15040875/72232234-b87bbf80-358d-11ea-9623-b83b766d931c.png">

<img width="876" alt="Screen Shot 2020-01-12 at 10 29 46 PM" src="https://user-images.githubusercontent.com/15040875/72231782-54f09280-358b-11ea-8101-115b1f266ed8.png">

<img width="878" alt="Screen Shot 2020-01-12 at 10 29 56 PM" src="https://user-images.githubusercontent.com/15040875/72231781-54f09280-358b-11ea-810e-7d183646f8bd.png">

<img width="1155" alt="Screen Shot 2020-01-12 at 10 30 14 PM" src="https://user-images.githubusercontent.com/15040875/72231780-54f09280-358b-11ea-873c-edfeeb265ad6.png">

<img width="1158" alt="Screen Shot 2020-01-12 at 10 30 27 PM" src="https://user-images.githubusercontent.com/15040875/72231779-54f09280-358b-11ea-8269-353db58a242e.png">

<img width="1153" alt="Screen Shot 2020-01-12 at 10 30 42 PM" src="https://user-images.githubusercontent.com/15040875/72231778-54f09280-358b-11ea-815e-973a4790c748.png">
