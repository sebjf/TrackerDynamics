function [] = plotPositions(T)

figure;
hold all;
ids = unique(T.id);
for i = ids'
    x = T.time(T.id==i,:);
    plot(x,T.position(T.id==i,:));
    plot(x,T.word(T.id==i,:));
end
    
end