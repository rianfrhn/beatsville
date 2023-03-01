 using Godot;
using System;

public class Projectile1 : Projectile
{
	//Kal's Projectile
	public Vector2 direction;
	int x1, y1;
	int x2, y2;
	int deltaX, deltaY;
	int signX, signY; 
	int e, a, b;
	bool interchange = false;
	public override void Move(int beat)
	{
		if(collided) return;
		Vector2 newPos = new Vector2();
		//Bresenham's line algorithm
		if (e <= 0)
		{
			if(interchange == true) { 
				newPos.y = signY; 
			} else { 
				newPos.x = signX;
			}
			e += a;
		}
		else
		{
			newPos.y = signY;
			newPos.x = signX;
			e += b;
		}
		GlobalPosition += newPos * 16;
		





	}
	public override void _EnterTree()
	{
		
		x1 = Mathf.RoundToInt((initialPos.x-8) / 16);
		x2 = Mathf.RoundToInt((targetpoint.x-8) / 16);
		y1 = Mathf.RoundToInt((initialPos.y-8) / 16);
		y2 = Mathf.RoundToInt((targetpoint.y-8) / 16);
		deltaX = Mathf.Abs(x2-x1);
		deltaY = Mathf.Abs(y2-y1);
		signX = Mathf.Sign(x2 - x1);
		signY = Mathf.Sign(y2 - y1);
		if (deltaY > deltaX)
		{
			int temp = deltaX;
			deltaX = deltaY;
			deltaY = temp;
			interchange = true;
		}


		e = 2 * deltaY - deltaX;
		a = 2 * deltaY;
		b = 2 * deltaY - 2 * deltaX;



	}
		



}
