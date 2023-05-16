from django.db import models
from django.contrib.auth.models import AbstractBaseUser, BaseUserManager

# Create your models here.

class UserManager(BaseUserManager):
    def create_user(self, email, password, **extra_fields):

        # Create and save a User with the given email and password.
        if not email:
            raise ValueError("Users must have an email address")
        email = self.normalize_email(email)
        user = self.model(email=email, **extra_fields)
        user.set_password(password)
        user.save()
        return user
    
    def create_superuser(self, email, password, **extra_fields):
        
        # Create and save a SuperUser with the given email and password.
        extra_fields.setdefault('is_staff', True)
        extra_fields.setdefault('is_active', True)
        extra_fields.setdefault('is_superuser', True)

        if extra_fields.get('is_staff') is not True:
            raise ValueError('Superuser must have is_staff=True.')
        
        return self.create_user(email, password, **extra_fields)

class User(AbstractBaseUser):
    email = models.EmailField(unique=True)
    first_name = models.CharField(max_length=35,null=True)
    is_staff = models.BooleanField(default=False)
    is_active = models.BooleanField(default=True)
    is_superuser = models.BooleanField(default=False)

    USERNAME_FIELD = 'email'
    objects = UserManager()

    def has_perm(self, perm, obj=None):
        # check if user is a superuser
        if self.is_superuser or self.is_staff:
            return True
        
        # regular users don't have any special permissions
        return False
    
    def has_module_perms(self, app_label):
        # check if user is a superuser
        if self.is_superuser or self.is_staff:
            return True
        
        # regular users don't have access to any app modules
        return False
    

    @property
    def total_score(self):
        scores = Scoreboard.objects.all().filter(user=self)
        total = 0
        for score in scores:
            total += score.score
        
        return total
    
    @property
    def average_score(self):
        scores = Scoreboard.objects.all().filter(user=self)
        
        if len(scores) > 0:
            return self.total_score / len(scores)
    
        return 0



class Scoreboard(models.Model):
    user = models.ForeignKey(User, on_delete=models.CASCADE, related_name='scoreboards')
    score = models.IntegerField(default=0)
    date = models.DateTimeField(auto_now=True)

    


    # extra fields here
    #leaderboard
    #total score of all users
    #average score of all users