# Twitter to RSS

Web app which converts recent tweets from a user into a RSS feed.

[![Deploy to Azure](http://azuredeploy.net/deploybutton.png)](https://azuredeploy.net/)

## Configuration

You need to define two configuration settings, either through the Azure Deploy or through Web.config:

* TWITTER_APP_ID
* TWITTER_APP_SECRET

## Usage
<pre><code>
Get["/twitter/{user}"]
</pre></code>

For example: http://myazure.com/twitter/@AdafyOy

## Contact

If you have any questions, you can contact [Mikael Koskinen](http://mikaelkoskinen.net) or [Adafy Oy](http://adafy.com).

## License

This software is distributed under the terms of the MIT License (see mit.txt).