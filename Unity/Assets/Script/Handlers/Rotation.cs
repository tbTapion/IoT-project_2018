public class Rotation {

    public float yaw, roll, pitch;
    public Rotation(float roll, float pitch, float yaw){
        this.yaw = yaw;
        this.roll = roll;
        this.pitch = pitch;
    }

    public void setRotation(float roll, float pitch, float yaw){
        this.yaw = yaw;
        this.roll = roll;
        this.pitch = pitch;
    }

    public void setRotation(Rotation rotation){
        setRotation()
    }
}