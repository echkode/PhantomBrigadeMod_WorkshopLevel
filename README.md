# WorkshopLevel

A library mod for [Phantom Brigade](https://braceyourselfgames.com/phantom-brigade/) to change the workshop level calculation.

It is compatible with game version **1.1.3** (Epic/Steam).

This is a demonstration mod to document how to create a library (code) mod from scratch and provide a basic template that can be easily reused.

I used Microsoft Visual Studio Community 2022 version 17.6.4 to create this mod.

First step is to create a new project. Open Visual Studio and then select the File > New > Project... menu item. This will bring up the create new project dialog.

![create new project dialog showing class library project selected](https://github.com/echkode/PhantomBrigadeMod_WorkshopLevel/assets/48565771/c9a446a5-8d95-44c0-ace8-174ddefe79ce)

The dialog is divided into two lists. If you've never used Visual Studio before, the list on the left is most likely going to be empty and you'll want to look at the list on the right. Above the list on the right are three dropdowns that will filter the available projects in the list. I've set the three dropdowns to `C#`, `Windows` and `Library` to reduce the list of project templates and then I scrolled down to select the one named "Class Library (.NET Framework)". The ".NET Framework" bit is important. There's a newer version of .NET that's not compatible with the older version used by Phantom Brigade. Click on `Next` to bring up the configure project screen.

![projection configuration screen with fields to name project and folder to save in](https://github.com/echkode/PhantomBrigadeMod_WorkshopLevel/assets/48565771/b58adb14-be2c-4c25-8f54-794dceea70a0)

The configure project screen is where you'll name the project and choose where to save it on your computer. The project will be saved in a new folder inside the folder in the Location field. For small projects like this mod, I save the solution and the project in the same folder to cut down on the number of folders in the project. Make sure the Framework field is set to ".NET Framework 4.7.2". Once you've entered all the information, click Create. You will be returned to Visual Studio with the new project opened.

![initial project layout with extra references and a starting class](https://github.com/echkode/PhantomBrigadeMod_WorkshopLevel/assets/48565771/d28cdb22-4a36-43e6-ab98-942ae4d617ac)

The first thing I do with a new project is fix the references. You will find these in the Solution Explorer tool window. Tool windows are small windows that are usually attached to the sides or bottom of the main window. The Solution Explorer tool window may appear on the right side of your setup and it may be collapsed -- that means it only shows up as a little tab on the very margins of the main window. If you can't see it, you can go to View > Solution Explorer to bring it up.

The Solution Explorer is a tree view that shows the files in your project and two other things: Properties and References. You'll want to expand References to see what ones have been added by default. You won't need any of the references from `System.Data` to the end of the list. You can remove a reference by right clicking on it and choosing `Remove`. You can also delete the file named "Class1.cs" while you're at it.

There are a few references that you will need to add. These are references to the assemblies that ship with the game. You can add references by right clicking on References and choosing `Add Reference...`. That will bring up the Add Reference Manager dialog.

![reference manager dialog showing required assemblies selected](https://github.com/echkode/PhantomBrigadeMod_WorkshopLevel/assets/48565771/99912e0c-54a3-4d1f-aec0-b39bb381dcfa)

In the screenshot you will see 4 assemblies (DLLs) with checkmarks to the left of their name. These checkmarks means the assemblies are selected and will be added as references when I click the OK button. You may not have any assemblies listed. In that case, you will need to click the Browse... button and then navigate to the location of the assemblies for the game. I'm using the Epic Games store so for me the game assemblies are found in `C:\Program Files\Epic Games\PhantomBrigade\PhantomBrigade_Data\Managed`. If you're using a stock install of Steam, you'll find them in `C:\Program Files (x86)\Steam\steamapps\common\Phantom Brigade\PhantomBrigade_Data\Managed`. If you have installed the game to a custom location and are having problems compiling the code in the project included with this repo, please see the [Advanced Topics section](#advanced-topics) for details on how to fix the project to work with your custom installation.

The 4 assemblies shown are the minimum needed to build a mod. For this mod, the Entitas assembly is required as well. After you have selected the mods and hit the OK button, you will need to add some files to the project.

![solution explorer showing the two standard mod source files](https://github.com/echkode/PhantomBrigadeMod_WorkshopLevel/assets/48565771/3e348baf-ca6c-4e31-a5b2-6d71f3501032)

There are two files I always add to mod projects where I'm going to patch functionality in the game. The first file is `ModLink.cs` which is largely boilerplate code that's necessary for the modding system to recognize my assembly as a mod. When I create a new mod project, I simply copy this file from one of my existing mod projects and only change the namespace. The second file is `Patch.cs` and this is where the I put all the Harmony patch functions. This file is specific to each mod.

I briefly mentioned the concept of namespace above. If you plan to distribute your mod to other people, I highly recommend that you make up a unique namespace. When you use a namespace, you can use common names for your classes without worrying about having a problem with other mods that might use those names too. I can use the names `ModLink` and `Patch` in all my mods without any problem because I create a unique namespace for each of my mods. I use `EchKode.PBMods` and the name of the mod to make those unique namespaces. This mod, for example, has the namespace `EchKode.PBMods.WorkshopLevel`. While not strictly necessary, I also change the default namespace for the project by right clicking on the `Properties` node in the Solution Explorer tree view and choosing `Open` to bring up the project properties window.

![project properties window with changed default namespace](https://github.com/echkode/PhantomBrigadeMod_WorkshopLevel/assets/48565771/6e6287e0-00dc-4cf8-a806-dfc14d5ee485)

In the [Discord forum post](https://discord.com/channels/380929397445754890/1147345439092375612/1147673029049073826) that inspired this mod, one of the game's programmers mentioned what function needs to be patched to change the workshop level calculation and the class that function can be found in. That may seem like useless information because you don't have direct access to the source code of the game but Phantom Brigade is shipped with unobfuscated assemblies. That means you can use a .NET disassembler like [dotPeek](https://www.jetbrains.com/decompiler/) to look up the class and see what the function looks like.

![dotPeek disassembly of PhantomBrigade.Overworld.Systems.OverworldBaseLevelSystem.RefreshBaseLevels](https://github.com/echkode/PhantomBrigadeMod_WorkshopLevel/assets/48565771/792efb60-abc1-4e86-9771-f8371427807b)

The patch code will go into the `Patch.cs` file. There are different ways to patch functions in the game. This mod uses a `Prefix` patch and replaces the entire function with my own C# code. You can also do much more surgical fixes with a `Transpiler` patch which works at the IL level (a low-level language in .NET). Go to the [Harmony documentation](https://harmony.pardeike.net/articles/intro.html) for more information about patching functions.

## Advanced Topics

This project won't compile if you have installed the game to a different location than the default one. To get this project to compile, you will need to edit the csproj file to change the path to the game. Close the project in Visual Studio. Next, open the `WorkshopLevel.csproj` file in a text editor and scroll down until you see the section of code that matches what's boxed in red in the screenshot.

![property group for game installation path](https://github.com/echkode/PhantomBrigadeMod_WorkshopLevel/assets/48565771/32c03d05-e320-4333-8e45-999ecceaac5d)

The section defines three variables named `SteamInstallationPath`, `EpicInstallationPath`, and `GameInstallationPath`. The first two point to the folder where the game is installed for Steam and Epic, respectively. If you have the game from only one of the stores, that's the variable you will want to change. You will want to change the text between the `>` and `<` characters to be the path to where the game is installed on your machine.

By default the Steam path is used. If you have the game from Epic, you will also have to change the `GameInstallationPath` variable. Replace the text between the `(` and `)` characters with `EpicInstallationPath`.

Once you have made these changes, save the file and exit the editor. Reopen the project in Visual Studio. Now the references will use the correct versions of the assemblies that match your game installation.
