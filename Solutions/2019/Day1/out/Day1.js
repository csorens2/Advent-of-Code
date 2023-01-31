"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var fs_1 = require("fs");
function get_input(filepath) {
    var file_contents = (0, fs_1.readFileSync)(filepath, 'utf-8');
    return file_contents.split(/\r?\n/).map(function (mass) { return parseInt(mass); });
}
function get_fuel_for_mass(mass) {
    return Math.floor(mass / 3) - 2;
}
function rec_get_fuel(next_mass) {
    var next_fuel_mass = get_fuel_for_mass(next_mass);
    if (next_fuel_mass < 0) {
        return 0;
    }
    return next_fuel_mass + rec_get_fuel(next_fuel_mass);
}
var input_file_path = __dirname + "/../input.txt";
var input = get_input(input_file_path);
var fuel_per_module = input.map(get_fuel_for_mass);
var part_1 = fuel_per_module.reduce(function (acc, next) { return acc + next; });
console.log("Part 1 Result ".concat(part_1));
var part_2 = fuel_per_module.
    map(function (starter_fuel) { return starter_fuel + rec_get_fuel(starter_fuel); }).
    reduce(function (acc, next) { return acc + next; });
console.log("Part 2 Result ".concat(part_2));
var test = 1;
//# sourceMappingURL=Day1.js.map