import { readFileSync } from 'fs';

function get_input(filepath) {
    const file_contents = readFileSync(filepath, 'utf-8');
    return file_contents.split(/\r?\n/).map(mass => parseInt(mass));
}

function get_fuel(mass) {
    return Math.floor(mass / 3) - 2;
}

function rec_get_fuel(next_mass) {
    const next_fuel_mass = get_fuel(next_mass);
    if (next_fuel_mass < 0) {
        return 0;
    }
    return next_fuel_mass + rec_get_fuel(next_fuel_mass);
}

const input_file_path = __dirname + "/../input.txt";
const input = get_input(input_file_path);

const fuel_per_module = input.map(get_fuel);

const part_1 = fuel_per_module.reduce((acc, next) => acc + next);
console.log(`Part 1 Result ${part_1}`)

const part_2 = fuel_per_module.
    map(starter_fuel => starter_fuel + rec_get_fuel(starter_fuel)).
    reduce((acc,next) => acc + next);
console.log(`Part 2 Result ${part_2}`);