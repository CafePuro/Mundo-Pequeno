/*
 * Created on Sat Sep 01 2018 by ZZZeMarcelo
 *
 * Copyright (c) 2018 Café Puro Digital Studio
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INoiseFilter 
{
	float Evaluate(Vector3 point);

}
