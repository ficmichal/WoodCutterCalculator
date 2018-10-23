function [y] = Power(x)
n = length(x);
for i = 1 : n
    y(i) = x(i).^2;
end
end

