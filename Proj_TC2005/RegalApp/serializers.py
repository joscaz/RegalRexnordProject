from rest_framework import serializers
from rest_framework.response import Response
from rest_framework import permissions, status

# User imports
from . models import User

# Scoreboard imports
from . models import Scoreboard

# Serializers here
class UserSerializer(serializers.HyperlinkedModelSerializer):
    total_score = serializers.CharField(read_only=True)
    average_score = serializers.CharField(read_only=True)
    
    class Meta:
        model = User
        fields = '__all__'

class LoginSerializer(serializers.Serializer):
    email = serializers.EmailField(
        required=True,
        style={'input_type': 'email', 'placeholder': 'Email'}
    )

    password = serializers.CharField(
        required=True,
        style={'input_type': 'password', 'placeholder': 'Password'}
    )


class ScoreboardSerializer(serializers.HyperlinkedModelSerializer):
    class Meta:
        model = Scoreboard
        fields = '__all__'

class ScoreSerializer(serializers.Serializer):
    score = serializers.IntegerField(required=True)



