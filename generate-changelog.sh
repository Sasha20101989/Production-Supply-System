#!/bin/sh



# Получение списка коммитов с сообщениями и авторами



commits_with_authors=$(git log --pretty=format:"%h - %s - %an" --no-merges)

commit_changelog="Updated CHANGELOG"



# Файл CHANGELOG.md, в который будут записываться изменения



changelog_file="CHANGELOG.md"







# Очистка файла CHANGELOG.md перед записью новых данных



echo "" > $changelog_file







# Заголовок CHANGELOG



echo "CHANGELOG" >> $changelog_file



echo "=========" >> $changelog_file



echo "" >> $changelog_file







# Раздел "Added"



echo "### Added " >> $changelog_file



echo "" >> $changelog_file







# Поиск коммитов с ключевыми словами в сообщении и запись в раздел "Added"



if grep -i -q "added" <<< "$commits_with_authors"; then



 grep -i "added" <<< "$commits_with_authors" >> $changelog_file



else



 echo "No commits with 'added' found." >> $changelog_file



fi







echo "" >> $changelog_file







#!/bin/sh







# Получение списка коммитов с сообщениями и авторами



commits_with_authors=$(git log --pretty=format:"%h - %s - %an" --no-merges)



commit_changelog="Updated CHANGELOG"







# Файл CHANGELOG.md, в который будут записываться изменения



changelog_file="CHANGELOG.md"







# Очистка файла CHANGELOG.md перед записью новых данных



echo "" > "$changelog_file"







# Заголовок CHANGELOG



echo "CHANGELOG" >> "$changelog_file"



echo "=========" >> "$changelog_file"



echo "" >> "$changelog_file"







# Раздел "Added"



echo "### Added " >> "$changelog_file"



echo "" >> "$changelog_file"







# Поиск коммитов с ключевыми словами в сообщении и запись в раздел "Added"



if grep -i -q "added" <<< "$commits_with_authors"; then



   grep -i "added" <<< "$commits_with_authors" | sed 's/^/ - /' >> "$changelog_file"



else



   echo "No commits with 'added' found." >> "$changelog_file"



fi







echo "" >> "$changelog_file"







# Раздел "Changed"



echo "### Changed " >> "$changelog_file"



echo "" >> "$changelog_file"







# Поиск коммитов с ключевыми словами в сообщении и запись в раздел "Changed"



if grep -i -q "changed" <<< "$commits_with_authors"; then



   grep -i "changed" <<< "$commits_with_authors" | sed 's/^/ - /' >> "$changelog_file"



else



   echo "No commits with 'changed' found." >> "$changelog_file"



fi







echo "" >> "$changelog_file"







# Раздел "Fixed"



echo "### Fixed " >> "$changelog_file"



echo "" >> "$changelog_file"







# Поиск коммитов с ключевыми словами в сообщении и запись в раздел "Fixed"



if grep -i -q "fixed" <<< "$commits_with_authors"; then



   grep -i "fixed" <<< "$commits_with_authors" | sed 's/^/ - /' >> "$changelog_file"



else



   echo "No commits with 'fixed' found." >> "$changelog_file"



fi







# Добавление CHANGELOG в индекс Git



git add "$changelog_file"