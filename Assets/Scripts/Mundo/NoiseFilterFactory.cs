/*
 * Created on Sat Sep 01 2018 by ZZZeMarcelo
 *
 * Copyright (c) 2018 Café Puro Digital Studio
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseFilterFactory 
{
	public static INoiseFilter CreateNoiseFilter(NoiseSettings settings)
	{
		switch(settings.filterType)
		{
			case NoiseSettings.FilterType.Simple:
				return new SimpleNoiseFilter(settings.simpleNoiseSettings);
			case NoiseSettings.FilterType.Ridgid:
				return new RidgidNoiseFilter(settings.ridgidNoiseSettings);
		}
		return null;

	}

}
