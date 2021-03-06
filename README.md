<!--
*** Thanks for checking out the Best-README-Template. If you have a suggestion
*** that would make this better, please fork the repo and create a pull request
*** or simply open an issue with the tag "enhancement".
*** Thanks again! Now go create something AMAZING! :D
-->



<!-- PROJECT SHIELDS -->
<!--
*** I'm using markdown "reference style" links for readability.
*** Reference links are enclosed in brackets [ ] instead of parentheses ( ).
*** See the bottom of this document for the declaration of the reference variables
*** for contributors-url, forks-url, etc. This is an optional, concise syntax you may use.
*** https://www.markdownguide.org/basic-syntax/#reference-style-links
-->
[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![Release][release-shield]][release-url]
[![Docker][docker-shield]][docker-url]
[![MIT License][license-shield]][license-url]



<!-- PROJECT LOGO -->
<br />
<p align="center">
  <!--<a href="https://github.com/othneildrew/Best-README-Template">
    <img src="images/logo.png" alt="Logo" width="80" height="80">
  </a>-->

  <h3 align="center">discord-event-bot</h3>

  <p align="center">
    Setup your own event planner.
    <br />
    <a href="https://github.com/Kreisverkehr/discord-event-bot"><strong>Explore the docs »</strong></a>
    <br />
    <br />
    <a href="https://github.com/Kreisverkehr/discord-event-bot/issues">Report Bug</a>
    ·
    <a href="https://github.com/Kreisverkehr/discord-event-bot/issues">Request Feature</a>
  </p>
</p>



<!-- TABLE OF CONTENTS -->
<details open="open">
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
  </ol>
</details>



<!-- ABOUT THE PROJECT -->
## About The Project

I alway wondered if there is an open source alternative to various event planning bots for discord. I couldn't find anything so I went on and created my own.

### Built With

* [Discord.NET](https://github.com/discord-net/Discord.Net)
* [Humanizer](https://github.com/Humanizr/Humanizer)
* [ICal.NET](https://github.com/rianjs/ical.net)
* [Quartz.NET](https://www.quartz-scheduler.net/)
* [.NET](https://github.com/dotnet)
* [EF Core](https://docs.microsoft.com/ef/core/)

<!-- GETTING STARTED -->
## Getting Started

You need to create a new discord application an get a bot Token. Follow this tutorial to get a bot token.
https://docs.stillu.cc/guides/getting_started/first-bot.html

### Installation

#### Option 1: Use docker-compose

The bot comes as a docker image. You can use the following samples to get it up and running in a matter of minutes. You can find sample docker-compose configurations below. Please note that you can either replace `${DiscordEventBotToken}` with your actual token or create an environment variable called `DiscordEventBotToken`.

docker-compose.yml (Linux containers)
```yaml
version: '3.4'

services:
  discordeventbot.service:
    image: kreisverkehr/discord-event-bot:latest
    environment:
        - DiscordEventBotToken=${DiscordEventBotToken}
        - DiscordEventBotDataStore=/mnt/datastore
    restart: always
    volumes:
        - datastore:/mnt/datastore
volumes:
  datastore:
```

docker-compose.yml (Windows containers)
```yaml
version: '3.4'

services:
  discordeventbot.service:
    image: kreisverkehr/discord-event-bot:latest
    environment:
        - DiscordEventBotToken=${DiscordEventBotToken}
        - DiscordEventBotDataStore=C:\volume
    restart: always
    volumes:
        - datastore:C:/volume
volumes:
  datastore:
```

#### Option 2: Manual install

Download and extract the appropriate zip folder for your environment and extract it. Run the excecutable. You will be asked a few questions and your bot is running.

<!-- ROADMAP -->
## Roadmap

See the [open issues](https://github.com/Kreisverkehr/discord-event-bot/issues) for a list of proposed features (and known issues).

<!-- CONTRIBUTING -->
## Contributing

Contributions are what make the open source community such an amazing place to be learn, inspire, and create. Any contributions you make are **greatly appreciated**.
Head over to Github and follow the steps. (https://github.com/Kreisverkehr/discord-event-bot)

1. Create an issue which describes the feature you are planning to implement
2. Fork the Project
3. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
4. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
5. Push to the Branch (`git push origin feature/AmazingFeature`)
6. Open a Pull Request

<!-- LICENSE -->
## License

Distributed under the MIT License. See `LICENSE` for more information.

<!-- CONTACT -->
## Contact

Project Link: [https://github.com/Kreisverkehr/discord-event-bot](https://github.com/Kreisverkehr/discord-event-bot)
Author: Kreisverkehr#5046 


<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[contributors-shield]: https://img.shields.io/github/contributors/Kreisverkehr/discord-event-bot.svg?logo=github&style=for-the-badge
[contributors-url]: https://github.com/Kreisverkehr/discord-event-bot/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/Kreisverkehr/discord-event-bot.svg?logo=github&style=for-the-badge
[forks-url]: https://github.com/Kreisverkehr/discord-event-bot/network/members
[stars-shield]: https://img.shields.io/github/stars/Kreisverkehr/discord-event-bot.svg?logo=github&style=for-the-badge
[stars-url]: https://github.com/Kreisverkehr/discord-event-bot/stargazers
[issues-shield]: https://img.shields.io/github/issues/Kreisverkehr/discord-event-bot.svg?logo=github&style=for-the-badge
[issues-url]: https://github.com/Kreisverkehr/discord-event-bot/issues
[license-shield]: https://img.shields.io/github/license/Kreisverkehr/discord-event-bot.svg?style=for-the-badge
[license-url]: https://github.com/Kreisverkehr/discord-event-bot/blob/main/LICENSE
[release-shield]: https://img.shields.io/github/downloads/Kreisverkehr/discord-event-bot/total?logo=github&style=for-the-badge
[release-url]: https://github.com/Kreisverkehr/discord-event-bot/releases/latest
[docker-shield]: https://img.shields.io/docker/pulls/kreisverkehr/discord-event-bot?logo=docker&style=for-the-badge
[docker-url]: https://hub.docker.com/r/kreisverkehr/discord-event-bot
[product-screenshot]: images/Homepage.png
