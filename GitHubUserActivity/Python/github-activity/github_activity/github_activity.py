"""
Configgure github-activity cli
"""

import click
from .app import fetch_activity

@click.command(name="github-activity")
@click.argument("username")
def cli(username):
    """
    github-activity cli
    """
    response = fetch_activity(username)
    for line in response:
        print(line)
