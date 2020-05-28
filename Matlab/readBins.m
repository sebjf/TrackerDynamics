function [] = readBins(directory)

if nargin < 1
   directory = fullfile(pwd,'..','Recordings');
end

for file = dir(fullfile(directory,'*.bin'))'
    T = readBin(fullfile(file.folder,file.name));
    [~,var,~] = fileparts(file.name);
    %evalin('base',[var '=' 'T' ';']);
    assignin('base',var,T);
end

end