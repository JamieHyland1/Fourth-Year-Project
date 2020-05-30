# School of Computing &mdash; Year 4 Project Proposal Form

> Edit (then commit and push) this document to complete your proposal form.
> Make use of figures / diagrams where appropriate.
>
> Do not rename this file.

## SECTION A

|                     |                   |
|---------------------|-------------------|
|Project Title:       | Game Theory: Modelling a simple society            |
|Student 1 Name:      | Jamie Hyland            |
|Student 1 ID:        | 17108152            |
|Student 2 Name:      | Alex Thornberry            |
|Student 2 ID:        | 15516603            |
|Project Supervisor:  | Alistair Sutherland     |

> Ensure that the Supervisor formally agrees to supervise your project; this is only recognised once the
> Supervisor assigns herself/himself via the project Dashboard.
>
> Project proposals without an assigned
> Supervisor will not be accepted for presentation to the Approval Panel.

## SECTION B

> Guidance: This document is expected to be approximately 3 pages in length, but it can exceed this page limit.
> It is also permissible to carry forward content from this proposal to your later documents (e.g. functional
> specification) as appropriate.
>
> Your proposal must include *at least* the following sections.


### Introduction


This project covers game theory, it deals with a 3d world where agents are employed to attempt to survive and reproduce. The agents behaviour is dynamic, this behaviour depends on given attributes in various situations. The agents will have a possibility of  mutation between generations. This is a project which will allow us to work with a malleable environment and with these changes, we can analyse and compare the various societies.

### Outline
This project aims to explore how a simple society evolves and grows over time. With a population with diverse attributes and behaviours. To start the project we need to set up the unity scene, to do this however, we will need to first have our art assets in order. We will be making a 3D landscape which will be populated with agents. We plan to introduce different types of terrain such as water, forests and mountains to diversify the landscape. These agents will have traits with continuous values including reproduction. This landscape is time dependant and every action will cost time and energy for said agents. This landscape will feature a predator prey type system where some agents are the apex predator and some are prey. Some agents will be codependent on other agents and will have to form a symbiotic relationship. This involves modelling what our agents look like in a 3D modelling software (Blender) packing the assets proper and then importing them into the unity editor. Once our 3D objects are successfully imported, shaded, textured and then placed in the scene we can then work on modelling the behaviours of the agents. Certain attributes will greatly alter the agents behaviour, this is a key aspect we will be focusing on analysing. 



### Background

The idea came from exploring autonomous agents such as boids, and genetic algorithms and trying to model real life structures and behaviours with code. This idea allows us to also explore game development as the tools needed to build such a project are apt for game development. The idea itself inherently allows for a lot of freedom and customization in its implementation which is something that was also very alluring to us.
This idea was encouraged through student 1’s previous experience using 3d visual libraries. Student 2 was immediately enthused by the idea and is happy to dive into an area of software engineering that he did not have any previous experience in.

### Achievements
Game theory is a way to study reasoning through mathematics. Game theory consists of the the various behaviour agents can display in a given circumstance, fully dependent on their given attributes. Game theory displays the rationality within the decision making of the agents based on the aforementioned circumstances. With our project specifically dealing with natural selection, the agents will do what is in the interests of their genes/attributes. This will lead to stronger and more favourable attributes being passed through the generations, with the weaker and less favourable attributes dying out. This can lead to interesting scenarios like parents sacrificing themselves for the sake of their children or agents more likely to cooperate with others with similar attributes.

We are hoping that our research will provide a greater understanding of cooperative agents working in tandem, adversarial agents competing for resources in a more selfish manner and symbiotic agents of different species who depend on one another for survival etc. We hope to extrapolate our data analysis to give a broad look into how humans behave, which can be used in economics.   


### Justification

One of the biggest justifications for this project was our ability to use tools like unity a tool for game development. This course tends not to focus on that area of software development and as such this idea gave us a good reason to delve into it while still relating to our course in an academic way. Also as someone (Student 1) who has previous experience using 3d Visual libraries and has a keen interest in creative coding this was a project that I found I could use as a conduit to further my skills in those areas. 

### Programming language(s)
C#
As its the default language for unity
R
Matlab
Python
These languages provide us with a means to carry out data analysis.


### Programming tools / Tech stack
Unity - render and compile our code.
Blender - free and open-source 3D computer graphics software toolset used for creating 3D printed models
R/Matlab/Python will be used as our tools for analysing our data


### Hardware

The minimum spec for the project is that a computer must be running an intel i5 4th generation cpu

Student 1:  Will be running on local machine that has intel i5 Generation 8 cpu
Student 2: Will be running on his local PC with an Nvidia 1060 graphics card (CPU: i5-4440K)

### Learning Challenges

We’re going to have to learn the unity engine, how the graphics pipeline works and how to manage a scene of 3d objects with lighting, AI, etc.

As our visualisation is in 3D we will have to use a 3d modelling tool such as blender to make some art assets.

Coming up with specific algorithms that implement the behaviours of our individual agents 

Learn to effectively extrapolate the continuous data and represent the data 

### Breakdown of work

There are a few aspects to this project;
Setting up the Unity environment
Creating the 3D landscape
Models for agents
Attributes and their variances 
Extrapolating data from the simulation
Analysing the data
Representing the data 
Altering/duplicating the environment, introducing new agents, attributes, mutations, etc.
Repeat from step 5


It has been mutually decided that both Students will take part in all aspects of this project. However, there are certain aspects of the project we think will be more successfully completed with more of a leading and supporting role. These aspects are highlighted below.

#### Student 1

Student 1 will be responsible for setting up the 3D landscape and main scene in the unity editor, this involves modelling the assets in Blender, packaging those assets correctly and importing them into unity and placing them in the scene. Will also be in charge of pathfinding for the agents and creating the class structure for the agents. 

#### Student 2


Student 2 will head the analytics for this project. He will be in charge of extrapolating the data of the societies. Will transfer these to a csv or text file. Will represent this data using graphs. Student 2 will lead the comparisons between different societies. Will make observations based on the varying dynamics and how they affect the ecosystem as a whole.




