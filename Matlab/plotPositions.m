function [] = plotPositions(T)

figure;
hold all;

labels = [];
mark_times = [];

ids = unique(T.id);
for i = ids'
    C = T(T.id==i,:); % controller table
    x = C.time;
   
    plot(x,C.position, 'LineWidth', 2); labels=[labels, "x","y","z"];
    plot(x(2:end),diff(C.position), 'LineWidth', 2); labels=[labels,"dx","dy","dz"];
    
    state_change = diff(C.word) ~= 0;
    mark_times = [mark_times; C.time(state_change)]; 
end

for t = mark_times'
   h = line([t t],ylim);
   h.Color = [0 0 0];
   h.LineStyle = ':';
end

legend(labels);


figure;
labels = [];
for i = ids'
    C = T(T.id==i,:); % controller table
    x = C.time;
   
    plot(x,abs(C.rotation), 'LineWidth', 2); labels=[labels, "w", "x","y","z"];
    %plot(x(2:end),diff(C.position), 'LineWidth', 2); labels=[labels,"dx","dy","dz"];
    
    state_change = diff(C.word) ~= 0;
    mark_times = [mark_times; C.time(state_change)]; 
end

for t = mark_times'
   h = line([t t],ylim);
   h.Color = [0 0 0];
   h.LineStyle = ':';
end

