"""
GitHub User Activity
"""

import requests


def fetch_activity(username):
    """
    Function to fetch user activity
    """
    url = f"https://api.github.com/users/{username}/events"
    response = requests.get(url, timeout=10)