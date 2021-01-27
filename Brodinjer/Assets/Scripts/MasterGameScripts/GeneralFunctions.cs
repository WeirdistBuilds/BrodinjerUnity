using UnityEngine;


public static class GeneralFunctions
{
    public static float ConvertRange(float origMinRange, float origMaxRange, float newMinRange, float newMaxRange, float value)
    {
        return (value - origMinRange) * (newMaxRange - newMinRange) / (origMaxRange - origMinRange) + newMinRange;
    }

    public static Vector3 ConvertRange(float origMinRange, float origMaxRange, Vector3 newMinRange, Vector3 newMaxRange, float value)
    {
        Vector3 newVector = Vector3.zero;
        newVector.x = (value - origMinRange) * (newMaxRange.x - newMinRange.x) / (origMaxRange - origMinRange) + newMinRange.x;
        newVector.y = (value - origMinRange) * (newMaxRange.y - newMinRange.y) / (origMaxRange - origMinRange) + newMinRange.y;
        newVector.z = (value - origMinRange) * (newMaxRange.z - newMinRange.z) / (origMaxRange - origMinRange) + newMinRange.z;
        return newVector;
    }

    public static float GetDirection(Vector3 targetPosition, Vector3 mainPosition)
    {
        Vector3 collisionposition = targetPosition;
        collisionposition.y = 0;
        Vector3 transformposition = mainPosition;
        transformposition.y = 0;
        Vector3 target = collisionposition - transformposition;
        float angle = Vector3.Angle(target, mainPosition);
        Vector3 crossProduct = Vector3.Cross(target, mainPosition);
        if (crossProduct.y < 0)
        {
            angle = -angle;
        }
        return angle;
    }

    public static bool CheckDestination(Vector3 Dest01, Vector3 Dest02, float offset)
    {
        if ((Dest01.x >= Dest02.x - offset
              && Dest01.x <= Dest02.x + offset)
            &&(Dest01.y >= Dest02.y - offset
                 && Dest01.y <= Dest02.y + offset)
            &&(Dest01.z >= Dest02.z - offset
                 && Dest01.z <= Dest02.z + offset))
        {
            return true;
        }
        return false;
    }
    
    public static bool CheckDestination(Vector3 Dest01, Vector3 Dest02, bool x, bool y, bool z, float offset)
    {
        if (((Dest01.x >= Dest02.x - offset
             && Dest01.x <= Dest02.x + offset) || !x)
            &&((Dest01.y >= Dest02.y - offset
               && Dest01.y <= Dest02.y + offset)||!y)
            &&((Dest01.z >= Dest02.z - offset
               && Dest01.z <= Dest02.z + offset)||!z))
        {
            return true;
        }
        return false;
    }

    public static void SetTimeScale(float val)
    {
        Time.timeScale = val;
    }
}
