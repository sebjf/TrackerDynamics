T = [];
for file = dir(fullfile(pwd,'..','Recordings','*.bin'))'
   T = [T; readBin(fullfile(file.folder,file.name))];
end
