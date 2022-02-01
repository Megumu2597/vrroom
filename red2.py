# -*- coding: utf-8 -*-
import cv2
import numpy as np
import socket

def red_detect(img):
    # HSV�F��Ԃɕϊ�
    hsv = cv2.cvtColor(img, cv2.COLOR_BGR2HSV)

    # �ԐF��HSV�̒l��1
    hsv_min = np.array([0,127,0])
    hsv_max = np.array([30,255,255])
    mask1 = cv2.inRange(hsv, hsv_min, hsv_max)

    # �ԐF��HSV�̒l��2
    hsv_min = np.array([150,127,0])
    hsv_max = np.array([179,255,255])
    mask2 = cv2.inRange(hsv, hsv_min, hsv_max)
    
    return mask1 + mask2

# �u���u���
def analysis_blob(binary_img):
    # 2�l�摜�̃��x�����O����
    label = cv2.connectedComponentsWithStats(binary_img)

    # �u���u�������ڕʂɒ��o
    n = label[0] - 1
    data = np.delete(label[2], 0, 0)
    center = np.delete(label[3], 0, 0)

    # �u���u�ʐύő�̃C���f�b�N�X
    max_index = np.argmax(data[:, 4])

    # �ʐύő�u���u�̏��i�[�p
    maxblob = {}

    # �ʐύő�u���u�̊e������擾
    maxblob["upper_left"] = (data[:, 0][max_index], data[:, 1][max_index]) # ������W
    maxblob["width"] = data[:, 2][max_index]  # ��
    maxblob["height"] = data[:, 3][max_index]  # ����
    maxblob["area"] = data[:, 4][max_index]   # �ʐ�
    maxblob["center"] = center[max_index]  # ���S���W
    
    return maxblob

def main():
	HOST = '127.0.0.1'
	PORT = 50007
	client = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
	image2=cv2.fillPoly(np.zeros((300,300,3),dtype=np.uint8),pts=[np.array([ [0,0], [0,300], [300, 300], [300,0] ])],color=(255,255,255))
	message2="hello"
	cap = cv2.VideoCapture(0)
	while(cap.isOpened()):
        # �t���[�����擾
		image2=cv2.fillPoly(np.zeros((300,300,3),dtype=np.uint8),pts=[np.array([ [0,0], [0,300], [300, 300], [300,0] ])],color=(255,255,255))
		ret, frame = cap.read()

        # �ԐF���o
		mask = red_detect(frame)

        # �}�X�N�摜���u���u��́i�ʐύő�̃u���u�����擾�j
		target = analysis_blob(mask)

        # �ʐύő�u���u�̒��S���W���擾
		center_x = int(target["center"][0])
		center_y = int(target["center"][1])
		menseki=int(target["area"])
        # �t���[���ɖʐύő�u���u�̒��S���͂��~�ŕ`��
		cv2.circle(frame, (center_x, center_y), 30, (0, 200, 0),
                   thickness=3, lineType=cv2.LINE_AA)
		cv2.putText(image2, str(message2),(100,150),cv2.FONT_HERSHEY_SIMPLEX, 1, ( 255, 0 ,0), 1, cv2.LINE_AA)
        # ���ʕ\��
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

        # q�L�[�������ꂽ��r���I��
		if cv2.waitKey(25) & 0xFF == ord('q'):
			break

	cap.release()
	cv2.destroyAllWindows()


if __name__ == '__main__':
    main() 