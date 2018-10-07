using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path 
{
	public readonly Tile[] lookPoints;
	public readonly Line[] turnBoundaries;
	public readonly int finishLineIndex;

	public Path(Tile[] waypoints, Tile startpos, float turnDst)
	{
		lookPoints = waypoints;
		turnBoundaries = new Line[lookPoints.Length];
		finishLineIndex = turnBoundaries.Length - 1;

		Vector2 previousPoint = new Vector2(startpos.x, startpos.y);
		for(int i = 0; i < lookPoints.Length; i++)
		{
			Vector2 currentPoint = new Vector2(lookPoints[i].x, lookPoints[i].y);
			Vector2 dirToCurrentPoint = (currentPoint - previousPoint).normalized;
			Vector2 turnBoundaryPoint = (i == finishLineIndex)? currentPoint : currentPoint - dirToCurrentPoint * turnDst;
			turnBoundaries[i] = new Line(turnBoundaryPoint, previousPoint - dirToCurrentPoint * turnDst);
			previousPoint = turnBoundaryPoint;
		}
	}

}
