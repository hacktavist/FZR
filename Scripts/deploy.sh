project ="FZR"



cd $(pwd)/Build

mkdir fzr
mv $(pwd)/windows/$project.exe fzr
mv $(pwd)/windows/$project_data fzr

echo "$(pwd)"
zip -r fzr.zip fzr


echo "Deploy to Itch"
wget http://dl.itch.ovh/butler/darwin-amd64/head/butler
chmod +x butler
touch butler_creds
echo -n $ITCH_API_KEY > butler_creds

./butler push fzr.zip hacktavist/fzr:windows -i butler_creds

echo "Cleaning"

./butler logout -i butler_creds --assume-yes
rm butler
