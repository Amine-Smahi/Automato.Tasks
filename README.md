<p align="center">
      <img src="https://api.nuget.org/v3-flatcontainer/automato.tasks/1.0.0/icon" height="80"/>
</p>
<h3 align="center">Automato.Tasks</h3>
<h5 align="center">The easiest tool to automate 🤖 tasks while you are sleeping 😴</h5>
<br/>


#### Requirements 
* .NET Core 3.1
#### Installation
* Open the terminal and run the command below
  
      user$ dotnet tool install --global Automato.Tasks
      
#### Get started
1) After the installation run the following command to create the appropiare environement

       user$ automato
       user$ Preparing...
             Finished!
2) Now simply fill up the tasks that you wish to execute while automato wait for a good internet connection to start downloading and executing tasks by executing

       user$  automato tasks
      Here is an exemple of some todos
      
       cmd => sudo apt-get update
       download => https://github.com/Amine-Smahi/Blood-Donation/archive/master.zip
       cmd => unzip master.zip
       cmd => cd master
       cmd => dotnet build

3) Launch automato by executing

       user$ automato
4) If you wish that your computer go to sleep after finishing all the tasks

       user$ automato true

#### Features
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

#### Note
* Feel free to open an issue,request a feature or contribute by opening a PR.
* This project is under MIT license
