# UniTrip — Explore Innopolis University in a Game

![logo](./assets/images/logo.jpeg)
**UniTrip** is an interactive Unity-based game that provides a virtual tour around the **Innopolis University**.  
Whether you're a student, parent, applicant, or just interested in the city — you can explore the atmosphere of university life from anywhere in the world.

The game offers detailed 2.5D locations, character interactions, quests, and even an **AI-powered chat guide** to answer your questions about Innopolis.

---


## Key Features

- **Two locations** with full functionality:
  - **Outer Hall**:  
    - 3 interactive NPCs (guards, students)  
    - Metal detector & turnstile simulation  
  - **Main Hall**:  
    - 2 NPCs  
    - 2 mini-quests:
      - **Auditorium 108**: Find hidden objects  
      - **Vending Zone**: Buy snacks or drinks  
    - AI-powered chat in information desk
- **Dialogue system** for professors, guards, and students  
- **Scene transitions** between areas  
- **Character movement** and trigger-based interaction zones   


### First dialogue
![interaction](assets/week3/dialog.png)
---



## Tech Stack

- **Unity** 2022.3.56f1 — Game engine  
- **Blender** — 3D models of environments and characters  
- **Procreate** — 2D assets: characters, items, UI  
- **C#** — Game logic and systems  
- **Python ML model** — Deployed via Hugging Face for AI-guide  

---

## Try It Now

You can play the latest version of UniTrip right in your browser:  
[**Play UniTrip Online**](https://iu-capstone-project-2025.github.io/UniTrip/)

---

## Local Development

To build or run the project locally:

1. Clone the repo:
   ```bash
   git clone https://github.com/IU-Capstone-Project-2025/UniTrip.git
2. Open the project with **Unity `2022.3.32f1` or later**.
3. Make sure the build target is set to **WebGL** (`File → Build Settings`).
4. Use `Build and Run` to test locally.