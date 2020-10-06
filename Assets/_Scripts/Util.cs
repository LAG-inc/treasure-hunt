using UnityEngine;

public static class Util
{
    /// <summary>
    /// Retorna el angulo entre dos puntos teniendo como parametros 
    /// </summary>
    /// <param name="p1">Posición numero 1</param>
    /// <param name="p2"></param>
    /// <returns></returns>
    public static float GetAngleFromTwoPoints(Vector3 p1, Vector3 p2)
    {
        var distanceX = p1.x - p2.x;
        var distanceY = p1.x - p1.y;
        //TangenteInversa en Radianes*Rad2Deg para obtener los grados en Deg
        var angle = Mathf.Atan2(distanceX, distanceY) * Mathf.Rad2Deg;
        return angle;
    }

    /// <summary>
    /// Encuentra el valor de un angulo a partir de un Vector
    /// </summary>
    /// <param name="lAngle"></param>
    /// <returns></returns>
    public static Vector3 GetVectorFromAngle(float lAngle)
    {
        var angleRad = lAngle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    /// <summary>
    /// Retorna el angulo de un vector
    /// </summary>
    /// <param name="lVector"></param>
    /// <returns></returns>
    public static float GetAngleFromVector(Vector3 lVector)
    {
        lVector = lVector.normalized;
        var n = Mathf.Atan2(lVector.y, lVector.x) * Mathf.Rad2Deg;
        n += n < 0 ? 360 : 0;
        return n;
    }
}