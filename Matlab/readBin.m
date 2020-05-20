function [ T ] = readBin(filename)
%READBIN Summary of this function goes here
%   Detailed explanation goes here
fid = fopen(filename, 'r');
v = fread(fid,Inf,'single');
fclose(fid);
v = reshape(v,10,[])';

id = v(:,1);
time = v(:,2);
position = v(:,3:5);
rotation = v(:,6:9);
word = v(:,10);

T = table(id,time,position,rotation,word);

end

