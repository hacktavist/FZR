#! /bin/sh

# Change this the name of your project. This will be the name of the final executables as well.
project="FZR"

echo "Attempting to build $project for Android"
/Applications/Unity/Unity.app/Contents/MacOS/Unity \
  -batchmode \
  -nographics \
  -silent-crashes \
  -logFile $(pwd)/unity.log \
  -projectPath $(pwd) \
  -buildTarget android # "$(pwd)/Build/windows/$project.exe" \
  -quit


echo 'Logs from build'
cat $(pwd)/unity.log
