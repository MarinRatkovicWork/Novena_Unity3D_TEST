# Novena_Unity3D_TEST
In this project you will develop a mobile application running on Android platform.
The minimum Android API level that application has to support is 21.
Layout must be responsive and support different screen sizes (Tablets included).
Screen orientation must be in Portrait.
Application must have the proper package name and icon (it's up to you what you will
choose)./br
Please use Unity 2021.3.5f1 version for this project.
Project must not contain any external dependencies (Assets, DLL’s). Only Unity developed
assets or plugins are allowed.
# DATA LOADING
All data must be loaded from the .json file. Json file and all other files (image, audio) must be
located and loaded from Application.persistentDataPath. A Json structure example file is
included with this document.
HINT: All of these files when we start the application can be downloaded from the server
(GitHub can serve for this purpose) and stored in Application.persistentDataPath. Another
alternative is to put them in Application.streamingAssetsPath and copy them into
Application.persistentDataPath.
# UI / WORKFLOW
Here is a short workflow about each page.
Application design file is included where you can see every page and all UI components that
you have to use.
Design must be implemented!
# PAGE 1 / LANGUAGE PAGE
On this page language buttons must be created based on the data that we have in Json.
When we choose a language our content must be set for the selected language.
Click/Tap on the button we have to go to the next page “List”.
# PAGE 2 / LIST PAGE
This page creates a list of buttons based on data in Json. Each button on click will lead to a
details page.
# PAGE 3 / DETAILS PAGE
On this page you need to load Images in the form of a gallery. Gallery has to change the
image every 5 seconds. Load audio file with progress bar that is interactable (we can seek
through audio taping on audio bar). Create an audio play/pause button. Load title in header.
Load detailed text about this point.
# PAGE 4 / EXTRA
This page is optional.
On this page try to implement something that you have been working on before it can be
anything. Maybe a simple game or interesting functionality.
Just add a button on the list page with text “Extra”.
# SUMMARY
This project requires you to develop content loading, ui navigation system, and media
interaction (audio, image).
Put this project on any version control service (GitHub, BitBucket …) and provide a link to it.
Do not send build .apk file.
Good luck!
Novena team wishes you happy coding :)
