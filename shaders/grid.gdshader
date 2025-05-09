shader_type canvas_item;

uniform vec2 resolution = vec2(1152, 648);
uniform vec4 line_color: source_color = vec4(1, 1, 1, 1);
uniform float grid_size = 16;
uniform float thickness: hint_range(0.0, 10.0, 0.1) = 1;

float draw_grid(vec2 uv) {
	vec2 grid_uv = cos(uv * TAU);
	return max(grid_uv.x, grid_uv.y);
}

void fragment() {
	vec2 uv = UV;
	uv.x *= resolution.x / resolution.y;
	float zoom = resolution.y / grid_size;
	float line_thickness = zoom * thickness / resolution.y;
	vec4 color = step(1.0 - line_thickness, draw_grid(uv * zoom)) * line_color;
	COLOR = color;
}
