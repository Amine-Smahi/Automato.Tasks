<p align="center">
      <img src="https://api.nuget.org/v3-flatcontainer/automato.tasks/1.0.0/icon" height="80"/>
</p>
<h3 align="center">Automato.Tasks</h3>
<h5 align="center">The easiest tool to automate 🤖 tasks while you are sleeping 😴</h5>
<br/>

### Challenges
* In the country where i live, you get durring the day an averrage internet speed of (40 kbps).
* The internet quality starts to get better at 2am.
* We need to plan the tasks that require a decent connexion at that time.
* Obviously, not everyone can stay that late.
### Solution
Create a tool that waits for the internet speed to get better(the minimum can be set from the settings) and start executing our tasks
- Download files 
- Execute commands

While we are sleeping.
### Requirements 
* .NET Core 3.1
### Technologies
* C#
* .NET Core
### Principles and patterns
* Separation of concerns pattern
* Don't repeat yourself principle
* Single Responsibility Principle
* Interface Segregation Principle
* Dependency Inversion Principle

### Installation
* Open the terminal and run the command below
  
      user$ dotnet tool install --global Automato.Tasks
      
### Get started
1) After the installation run the following command to create the appropriate environment

       user$ automato
2) To update your tasks run the following command

       user$ automato tasks
      Here is an exemple of some todos
      
       cmd => sudo apt-get update
       download => https://github.com/Amine-Smahi/Blood-Donation/archive/master.zip
       cmd => unzip master.zip
       cmd => cd master
       cmd => dotnet build

3) Launch automato by executing

       user$ automato
4) If you wish that your computer go to sleep after finishing all the tasks

       user$ automato sleep

### Features
- All automato settings are easy to configure, simply edit the <b>settings.json</b> file

            {
              "SettingsFileLocation": "./settings.json",
              "DownloadLocation": "./downloads",
              "MinimumInternetSpeed": 30,
              "MinimumGoodPings": 5,
              "TasksLocation": "./MyTasks.txt",
              "TaskTypeSplitter": "=\u003E",
              "WaitFewSecondsForAnotherTry": 2000
            }
- To open the <b>settings.json</b> file, run

       user$ automato settings
- To open the <b>downloads</b> folder, run

        user$ automato downloads

### Note
* Feel free to open an issue, request a feature or contribute by opening a PR.
* This project is under MIT license
