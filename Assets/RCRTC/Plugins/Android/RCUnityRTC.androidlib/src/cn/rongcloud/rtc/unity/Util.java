//<#CLASS_NAME=JavaUtils#>
package cn.rongcloud.rtc.unity;

import android.graphics.Bitmap;
import android.graphics.BitmapFactory;

import java.net.URI;

public class Util {
    public static Bitmap loadBitMap(String path) {
        Bitmap bitmap = null;
        try {
            bitmap = BitmapFactory.decodeFile(path);
        } catch (Exception e) {
            e.printStackTrace();
        }
        return bitmap;
    }
}
