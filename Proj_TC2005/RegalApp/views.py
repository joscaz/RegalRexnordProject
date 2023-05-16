from django.http import HttpResponse
from rest_framework import viewsets, status
from rest_framework.response import Response
from rest_framework.decorators import action
from rest_framework.exceptions import ValidationError
from django.contrib.auth import login, logout, authenticate
from django.contrib.auth.hashers import check_password
from django.utils import timezone
from rest_framework.authentication import SessionAuthentication, TokenAuthentication
from rest_framework.permissions import AllowAny, IsAuthenticated, IsAdminUser 
from rest_framework.authtoken.models import Token
from django.contrib.auth.models import AnonymousUser



# User imports
from . models import User 
from . serializers import UserSerializer

# Scoreboard imports
from . models import Scoreboard
from . serializers import ScoreboardSerializer, ScoreSerializer

# Login imports
from . serializers import LoginSerializer

# Views here
def index(request):
    return HttpResponse(" ")

# Viewsets that allow for CRUD operations
class UserView(viewsets.ModelViewSet):
    queryset = User.objects.all() # SELECT * FROM User
    serializer_class = UserSerializer
    authentication_classes = (SessionAuthentication, TokenAuthentication)
    permission_classes = (IsAuthenticated, IsAdminUser)

    #Log in
    @action(methods=["POST"], detail=False, serializer_class=LoginSerializer, permission_classes=[AllowAny])
    def log_in(self, request):
        serializer = LoginSerializer(data=request.data)
        serializer.is_valid(raise_exception=True)
        email = serializer.validated_data["email"]
        password = serializer.validated_data["password"]

        user = authenticate(email=email, password=password)
        if user is None:
            raise ValidationError({"error": "Invalid credentials"})

        token, _ = Token.objects.get_or_create(user=user)
        user.last_login = timezone.now()
        user.auth_token = token
        user.save()

        # Return the response with the user and token information
        return Response({
            "user": str(user),
            "token": token.key,
            "user_id": user.pk,
            "total_score": user.total_score,
            "average_score": user.average_score,
        }, status=status.HTTP_200_OK)

    # Sign up / Create user
    def create(self, request):
        serializer = UserSerializer(data=request.data)
        user = None

        if serializer.is_valid():
            user = User.objects.create_user(
                email=serializer.validated_data["email"],
                password=serializer.validated_data["password"],
                first_name=serializer.validated_data["first_name"],
            )

            # Last login set explicitly
            user.last_login = timezone.now()
            user.save()
            response = UserSerializer(instance=user, context={"request": request})
            return Response(response.data, status=status.HTTP_200_OK)
          
        else:
            return Response(serializer.errors, status=status.HTTP_400_BAD_REQUEST)
        
    # Log out
    @action(methods=["POST"], detail=False, serializer_class=LoginSerializer, permission_classes=[AllowAny])
    def log_out(self, request):        
        try:
            request.user.auth_token.delete()

        except (AttributeError, Token.DoesNotExist):
            pass # Token does not exist

        return Response({"detail" : "Logged out"}, status=status.HTTP_200_OK)
        
class ScoreboardView(viewsets.ModelViewSet):
    queryset = Scoreboard.objects.all() 
    serializer_class = ScoreboardSerializer
    authentication_classes = (SessionAuthentication, TokenAuthentication)
    permission_classes = (IsAuthenticated, IsAdminUser)

    @action(methods=["POST"], detail=False, serializer_class=ScoreSerializer, permission_classes=[IsAuthenticated])

    # Register score
    def register_score(self, request):
        serializer = ScoreSerializer(data=request.data)

        if serializer.is_valid():
            score = serializer.validated_data["score"]
            score_instance = Scoreboard.objects.create(user=request.user, score=score)
            # user = User.objects.get(email=request.user)
            # score = Scoreboard.objects.create(
            #     user=user,
            #     score=serializer.validated_data["score"]
            # )
            # score.save()

            return Response({"detail" : "Score saved"}, status=status.HTTP_200_OK)
        
        else:
            return Response(serializer.errors, status=status.HTTP_400_BAD_REQUEST)