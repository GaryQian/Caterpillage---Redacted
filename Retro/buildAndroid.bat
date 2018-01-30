del C:\Lua\love-android\assets\game.love
::copy tmhq\* love-android\assets\


set FILETOZIP=C:\Lua\tmhq\*


set TEMPDIR=C:\temp738
del /q %TEMPDIR%\*
rmdir %TEMPDIR%
mkdir %TEMPDIR%
xcopy /s /y %FILETOZIP% %TEMPDIR%

echo Set objArgs = WScript.Arguments > _zipIt.vbs
echo InputFolder = objArgs(0) >> _zipIt.vbs
echo ZipFile = objArgs(1) >> _zipIt.vbs
echo CreateObject("Scripting.FileSystemObject").CreateTextFile(ZipFile, True).Write "PK" ^& Chr(5) ^& Chr(6) ^& String(18, vbNullChar) >> _zipIt.vbs
echo Set objShell = CreateObject("Shell.Application") >> _zipIt.vbs
echo Set source = objShell.NameSpace(InputFolder).Items >> _zipIt.vbs
echo objShell.NameSpace(ZipFile).CopyHere(source) >> _zipIt.vbs
echo wScript.Sleep 2000 >> _zipIt.vbs

CScript  _zipIt.vbs  %TEMPDIR%  C:\Lua\love-android\assets\game.zip

rename C:\Lua\love-android\assets\game.zip game.love
::cd /d C:\Lua\love-android
::ant debug
cd /d C:\Lua\love-android
start compileAPK
timeout 10
::del C:\Lua\love-android\bin\caterpillage.apk
::rename C:\Lua\love-android\bin\love_android_sdl2-debug.apk caterpillage.apk
cd /d C:\Lua\love-android\bin
adb install love_android_sdl2-debug.apk