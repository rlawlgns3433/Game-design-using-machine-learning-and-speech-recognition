# message rule : [id,recognized_text] STT_BOX1
# 실행할 때 반드시 인자로 ID값을 줘야 함
# ex) py path/to/STT.py myid 

import speech_recognition as sr
import redis
import sys
import pyaudio

# Redis Server에 접속
rd=None
svr = ['10.20.11.16',6006]
# 음성 인식기 초기화

#api_key = "AIzaSyCcdpUD3exNjx1vl4ILUXycq-QqdgToGyY"

STT_channel = 'STT_BOX1'

rd = redis.Redis(host = svr[0], port = svr[1])
r = sr.Recognizer()

print("calibrating...")

with sr.Microphone() as source :
    r.adjust_for_ambient_noise(source)

def SpeakAnyTime() :
    # 마이크 사용
    with sr.Microphone() as source:
        # 1초간 소음 정도를 측정
        print("Please talk...")
        r.adjust_for_ambient_noise(source)

        # 마이크로부터 음성을 인식
        audio_data = r.listen(source)

        print("Recognizing...")

        # Google Web Speech API를 이용해 음성을 문자로 변환, Redis Server에 변환된 문자를 publish
        try : 
            text = sys.argv[1] + ',' + r.recognize_google(audio_data,language = 'ko-KR')

            rd.publish(channel= STT_channel, message=text)
        except : 
            text = ""
        
# 계속해서 입력을 받음
while True : 
    SpeakAnyTime()
