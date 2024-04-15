# Referral Payment System

Welcome to the Referral Payment System! This project is a web-based application 
that allows users to make payments to the another person without the use of 
the recipients contact number but the referral code of the recipient which is
 the referral code users input on login. 
 
 Users are also assigned referral codes once logged in but will have to contact 
 support to enable their account be changed to {Business} so they also be sent 
 funds using their referral code. Business accounts will have stripe connected 
 accounts where from there, they can send their received funds to their personal bank account.
 
 Administrative access is to only users who log-on with a referral code of a user 
 who has a role of  {Admin} and such a user has privileges of deleting other 
 users, making an account a {business} account or an {Admin} account and getting 
 all users of the product
 
The application leverages referral codes for transactions, enabling a network 
of referrals and payments within the system.
Using the Stripe payment API, recipients receive their funds instantly.


## Table of Contents
Front Project       =>Controllers - that take user interaction from the user interface.
                            =>Proxys - Redirects user requests to the appropriate API's on 
                            the Referral project.
                            =>View - What the UI looks like which is advised to change per usage
                            
Referral Project   => Models - A collection of fields that pass info around 
                            => DTOs - Data Transfer Objects. Unlike models, they only contains
                             what the user needs at a given point in time which giving too much
                             => Repositories - Act as an abstraction to the database being used 
                             thus instead of interacting with the database directing, interact 
                             with the repositories and let it handle your database communications
                             => Settings - Contains set-up settings to postGres database and to Stripe API's
                             => Extensions - Constants and statics used throughout the project. Constants
                              that can and should be run once project runs thus can be called anywhere
                              => Endpoints - Where all requests made by users have their base 
                              implementation thus the endpoint of all requests as in where 
                              the request in actually made
                              => Services = Endpoints to the payment platforms and settings, 
                              endpoints of the referral code assignment and endpoint of 
                              the excel download of all data from the database on users
                              => Controller - Only one controller here that takes the proxy 
                              request from the Front project and sees to the appropriate 
                              endpoint taking up the request
                              => appsettings.json - the external API's have the keys setUp 
                              here so its part of the configuration of the project 
                              => Program.cs - Register all services used and control the flow 
                              of the project. Also would handle the pipeline if you wanna go LIVE.


## Implementation
On the home page, the user is required to input 4 fields, first name, last name, 
his telephone number and the referral code of the recipients of the funds to be sent.

Once this is done, he to redirected to the page where he inputs the amount to 
be paid. Once that is done and payment clicked, he is directed to the payment 
platform which is stripe to authorize payment using his credit/debit/prepaid 
card details.

A successful payment will give him a page confirming success and unsuccessful 
payment will also be notified on the page.

If a user uses a referral code that has the role of admin, he has access to 
admin features where he can manage other users accounts in making them 
businesses, admins also, deleting them or getting all their details. He can also 
get to download all the data logged on the database since the product went LIVE unless deleted.

If a business is registered as a business, he is given a link to complete his stripe 
connected account registration to be able to receive funds. This will is a 
procedure where the user will be confirmed and connected account setup within 
not more than 5minutes.

#NOTE _ This is mainly a backend project and in use, only use the Referral project from 
the solution. The Front Project is just to handle how the project would look. 
All the API's, Endpoints and Services are on the referral project. The Front Project contains 
the proxies from the user controllers and authorization implementation which has 
its API's in the Referral Project. The front project is just a front.
