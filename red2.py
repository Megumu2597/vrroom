# -*- coding: utf-8 -*-
import cv2
import numpy as np
import socket

def red_detect(img):
    # HSV色空間に変換
    hsv = cv2.cvtColor(img, cv2.COLOR_BGR2HSV)

    # 赤色のHSVの値域1
    hsv_min = np.array([0,127,0])
    hsv_max = np.array([30,255,255])
    mask1 = cv2.inRange(hsv, hsv_min, hsv_max)

    # 赤色のHSVの値域2
    hsv_min = np.array([150,127,0])
    hsv_max = np.array([179,255,255])
    mask2 = cv2.inRange(hsv, hsv_min, hsv_max)
    
    return mask1 + mask2

# ブロブ解析
def analysis_blob(binary_img):
    # 2値画像のラベリング処理
    label = cv2.connectedComponentsWithStats(binary_img)

    # ブロブ情報を項目別に抽出
    n = label[0] - 1
    data = np.delete(label[2], 0, 0)
    center = np.delete(label[3], 0, 0)

    # ブロブ面積最大のインデックス
    max_index = np.argmax(data[:, 4])

    # 面積最大ブロブの情報格納用
    maxblob = {}

    # 面積最大ブロブの各種情報を取得
    maxblob["upper_left"] = (data[:, 0][max_index], data[:, 1][max_index]) # 左上座標
    maxblob["width"] = data[:, 2][max_index]  # 幅
    maxblob["height"] = data[:, 3][max_index]  # 高さ
    maxblob["area"] = data[:, 4][max_index]   # 面積
    maxblob["center"] = center[max_index]  # 中心座標
    
    return maxblob

def main():
	HOST = '127.0.0.1'
	PORT = 50007
	client = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
	image2=cv2.fillPoly(np.zeros((300,300,3),dtype=np.uint8),pts=[np.array([ [0,0], [0,300], [300, 300], [300,0] ])],color=(255,255,255))
	message2="hello"
	cap = cv2.VideoCapture(0)
	while(cap.isOpened()):
        # フレームを取得
		image2=cv2.fillPoly(np.zeros((300,300,3),dtype=np.uint8),pts=[np.array([ [0,0], [0,300], [300, 300], [300,0] ])],color=(255,255,255))
		ret, frame = cap.read()

        # 赤色検出
		mask = red_detect(frame)

        # マスク画像をブロブ解析（面積最大のブロブ情報を取得）
		target = analysis_blob(mask)

        # 面積最大ブロブの中心座標を取得
		center_x = int(target["center"][0])
		center_y = int(target["center"][1])
		menseki=int(target["area"])
        # フレームに面積最大ブロブの中心周囲を円で描く
		cv2.circle(frame, (center_x, center_y), 30, (0, 200, 0),
                   thickness=3, lineType=cv2.LINE_AA)
		cv2.putText(image2, str(message2),(100,150),cv2.FONT_HERSHEY_SIMPLEX, 1, ( 255, 0 ,0), 1, cv2.LINE_AA)
        # 結果表示
		if (center_x)>460:
			message2="left"
		elif (center_x)<180:
			message2="right"
		elif (center_y)<180:
			message2="up"
		elif (center_y)>300:
			message2="down"
		else:
			message2="stop"
		if (menseki<10000):
			message2="stop"
		cv2.imshow("Frame", frame)
		#cv2.imshow("Mask", mask)
		cv2.imshow("2",image2)
		client.sendto(message2.encode('utf-8'),(HOST,PORT))

        # qキーが押されたら途中終了
		if cv2.waitKey(25) & 0xFF == ord('q'):
			break

	cap.release()
	cv2.destroyAllWindows()


if __name__ == '__main__':
    main() 