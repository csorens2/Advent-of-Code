"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const fs_1 = require("fs");
function get_input(filepath) {
    let input_data = (0, fs_1.readFileSync)(filepath, 'utf-8').
        split(/\r?\n/).
        map(line => line.split(",")).
        map(wire => wire.map(single_wire => [single_wire[0], parseInt(single_wire.substring(1))]));
    return input_data;
}
function tuple_to_key(key) {
}
function part_one(input) {
    let points_map = new Map();
    input.forEach(function (wire) {
        let x_point = 0;
        let y_point = 0;
        wire.forEach(function (wire_inst) {
            const direction_funcs = {
                'U': (x, y) => [x, y + 1],
                'D': (x, y) => [x, y - 1],
                'R': (x, y) => [x + 1, y],
                'L': (x, y) => [x - 1, y],
            };
            const wire_func = direction_funcs[wire_inst[0]];
            for (let i = 0; i < wire_inst[1]; i++) {
                x_point = wire_func(x_point, y_point)[0];
                y_point = wire_func(x_point, y_point)[1];
                let x_y_key = `${x_point},${y_point}`;
                if (points_map.has(x_y_key)) {
                    points_map.set(x_y_key, points_map.get(x_y_key) + 1);
                }
                else {
                    points_map.set(x_y_key, 1);
                }
            }
        });
    });
    let toReturn = Array.from(points_map.entries()).
        filter(entry => entry[1] > 1).
        map(entry => entry[0].split(",").map(point => parseInt(point))).
        map(tuples => Math.abs(tuples[0] + Math.abs(tuples[1]))).sort((a, b) => {
        if (a < b) {
            return -1;
        }
        if (a > b) {
            return 1;
        }
        return 0;
    });
    return 0;
}
function part_two() {
}
const input_file_path = __dirname + "/../input.txt";
const input = get_input(input_file_path);
const part_one_result = part_one(input);
console.log(`Part 1 Result: ${part_one_result}`);
//const part_two_result = part_two();
//console.log(`Part 2 Result: ${part_two_result}`);
//# sourceMappingURL=Day3.js.map