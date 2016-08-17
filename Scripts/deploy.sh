FILENAME=“Users/travis/build/hacktavist/FZR/Build/windows”

zip -r "$FILENAME.zip" $FILENAME

echo "Deploy to Itch"
wget http://dl.itch.ovh/butler/darwin-amd64/head/butler
chmod +x butler
touch butler_creds
echo -n $ITCH_API_KEY > butler_creds

./butler push $(pwd).zip hacktavist/testing-travisci-deployment:windows -i butler_creds

echo "Cleaning"

./butler logout -i butler_creds --assume-yes
rm butler
