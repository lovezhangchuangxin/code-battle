// fragment shader 代码
shader_type canvas_item;

uniform sampler2D noise_texture;
uniform vec4 base_color: source_color = vec4(0.5, 0.5, 0.5, 1.0); // 基础灰色
uniform float noise_strength : hint_range(0, 1) = 0.1;

void fragment() {
    // 采样噪声（UV 可缩放）
    vec2 uv = UV * 5.0; // 控制纹理密度
    float noise_val = texture(noise_texture, uv).r;
    
    // 混合基础色和噪声
    vec3 color = base_color.rgb;
    color += (noise_val - 0.5) * noise_strength; // 让噪声有正负变化
    
    COLOR = vec4(color, base_color.a);
}