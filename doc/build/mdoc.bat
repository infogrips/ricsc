@echo off

set base_dir=%~dp0

echo %base_dir%

e:\utility\ics.exe -script %base_dir%\mdoc.cfg -base_directory %base_dir% %*
