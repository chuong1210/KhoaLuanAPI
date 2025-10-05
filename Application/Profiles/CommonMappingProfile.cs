using Application.DTOs.Banner;
using Application.DTOs.Shop;
using Application.DTOs.Cart;

using Application.DTOs.Transfer;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Profiles
{
    public class CommonMappingProfile: Profile
    {
        public CommonMappingProfile()
        {
            CreateMap<Shop, ShopDto>()
                .ForMember(d => d.WalletAmount, opt => opt.MapFrom(s => s.Wallet != null ? s.Wallet.Amount : 0))
                .ForMember(d => d.FollowerCount, opt => opt.Ignore())
                .ForMember(d => d.IsFollowing, opt => opt.Ignore());

            CreateMap<CreateShopRequest, Shop>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => Guid.NewGuid().ToString()))
                .ForMember(d => d.ShopStatus, opt => opt.MapFrom(s => true));

            CreateMap<UpdateShopRequest, Shop>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));


            CreateMap<Banner, BannerDto>();

            CreateMap<CreateBannerRequest, Banner>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => Guid.NewGuid().ToString()));

            CreateMap<UpdateBannerRequest, Banner>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<ProgressTransfer, ProgressTransferDto>();


            CreateMap<ClientTransfer, ClientTransferDto>();

            CreateMap<CreateProgressTransferRequest, ProgressTransfer>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => Guid.NewGuid().ToString()));


            CreateMap<OfflineTransaction, OfflineTransactionDto>();

            CreateMap<CreateOfflineTransactionRequest, OfflineTransaction>();

            CreateMap<Cart, CartDto>()

               .ForMember(d => d.ShopGroups, opt => opt.Ignore());

            CreateMap<CartItem, CartItemDto>();
      
        }
    }
    }
