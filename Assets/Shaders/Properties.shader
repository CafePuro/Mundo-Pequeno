Shader "Exercise/Properties"
{
	Properties
	{
		_Color("My Color", Color) = (1,1,1,1)
		_Position("My Position", Vector) = (1,1,1,1)
		_Transparency("My Transparency", Range(0,1)) = 1
		_Distance("My Distance", Float) = 1
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM
			half4 _Color;
			float4 _Position;
			fixed _Transparency;
			float _Distance;




			ENDCG
		}
	}
}
