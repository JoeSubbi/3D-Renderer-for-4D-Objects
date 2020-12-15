public class Vector {
    double x = 0;
    double y = 0;
    double z = 0;

    public Vector(double x, double y, double z){
        double length = Math.sqrt(x*x + y*y + z*z);
        if (length>0) {
            this.x = x / length;
            this.y = y / length;
            this.z = z / length;
        }
    }

    Vector CrossProduct(Vector v){
        Vector CrossVector = new Vector(
                y*v.z - z*v.y,
                z*v.x - x*v.z,
                x*v.y - y*v.x);
        return CrossVector;
    }
}
